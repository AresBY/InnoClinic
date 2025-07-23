const apiBase = 'http://localhost:5179/api';

export const environment = {
  production: false,
  apiUrl: apiBase,
  authUrl: `${apiBase}/Auth`,
  doctorAuthUrl: `${apiBase}/DoctorAuth`
};
