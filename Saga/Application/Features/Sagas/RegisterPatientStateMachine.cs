using Automatonymous;

using MassTransit;

namespace InnoClinic.Saga.Application.Features.Sagas
{

    // Messages/events definitions
    public record RegisterPatient(Guid CorrelationId, string Email);
    public record ProfileCreated(Guid CorrelationId, Guid ProfileId);
    public record ProfileCreationFailed(Guid CorrelationId, string Reason);

    // Command sent to Profiles microservice to create patient profile
    public record CreatePatientProfile
    {
        public Guid CorrelationId { get; init; }
        public string Email { get; init; } = null!;
    }

    public class RegisterPatientStateMachine : MassTransitStateMachine<RegisterPatientState>
    {
        public State WaitingForProfile { get; private set; }
        public State Completed { get; private set; }
        public State Failed { get; private set; }

        public Event<RegisterPatient> RegisterPatientEvent { get; private set; }
        public Event<ProfileCreated> ProfileCreatedEvent { get; private set; }
        public Event<ProfileCreationFailed> ProfileFailedEvent { get; private set; }

        public RegisterPatientStateMachine()
        {
            // Map saga state property
            InstanceState(x => x.CurrentState);

            // Configure events and correlation by CorrelationId
            Event(() => RegisterPatientEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ProfileCreatedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));
            Event(() => ProfileFailedEvent, x => x.CorrelateById(m => m.Message.CorrelationId));

            // Initial state when patient registration event is received
            Initially(
                When(RegisterPatientEvent)
                    .Then(ctx =>
                    {
                        ctx.Instance.Email = ctx.Data.Email;
                        ctx.Instance.CorrelationId = ctx.Data.CorrelationId;
                        Console.WriteLine($"[Saga] Register request received for {ctx.Data.Email}");
                    })
                    .Publish(ctx => new CreatePatientProfile
                    {
                        CorrelationId = ctx.Instance.CorrelationId,
                        Email = ctx.Instance.Email
                    })
                    .TransitionTo(WaitingForProfile)
            );

            // Waiting for profile creation result
            During(WaitingForProfile,
                When(ProfileCreatedEvent)
                    .Then(ctx =>
                    {
                        ctx.Instance.ProfileId = ctx.Data.ProfileId;
                        Console.WriteLine($"[Saga] Profile created with ID {ctx.Instance.ProfileId}");
                    })
                    .TransitionTo(Completed),

                When(ProfileFailedEvent)
                    .Then(ctx =>
                    {
                        Console.WriteLine($"[Saga] Profile creation failed: {ctx.Data.Reason}");
                        //here you can make a rollback if necessary
                    })
                    .TransitionTo(Failed)
            );

            // Logging for final states
            DuringAny(
                When(Completed.Enter)
                    .Then(ctx => Console.WriteLine($"[Saga] Patient registration completed successfully: {ctx.Instance.Email}")),

                When(Failed.Enter)
                    .Then(ctx => Console.WriteLine($"[Saga] Patient registration failed: {ctx.Instance.Email}"))
            );
        }
    }
}
