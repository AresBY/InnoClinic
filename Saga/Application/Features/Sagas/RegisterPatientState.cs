using Automatonymous;

namespace InnoClinic.Saga.Application.Features.Sagas
{
    public class RegisterPatientState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = null!;
        public Guid? ProfileId { get; set; }
        public string Email { get; set; } = null!;
    }

}
