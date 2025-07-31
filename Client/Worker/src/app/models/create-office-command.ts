export interface CreateOfficeCommand {
  photoUrl?: string;
  city: string;
  street: string;
  houseNumber: string;
  officeNumber?: string;
  registryPhoneNumber: string;
  status: boolean;
}
