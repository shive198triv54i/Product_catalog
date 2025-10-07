export const isEmailValid = (email: string): boolean => {
  const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return regex.test(email);
};

export const isPasswordStrong = (password: string): boolean => {
  const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;
  return regex.test(password);
};

export const isNameValid = (name: string): boolean => {
  return name.trim().length >= 2;
};

export const isPhoneValid = (phone: string): boolean => {
  const regex = /^[0-9]{10}$/;
  return regex.test(phone);
};

export const isPincodeValid = (pincode: string): boolean => {
  const regex = /^[1-9][0-9]{5}$/;
  return regex.test(pincode);
};
