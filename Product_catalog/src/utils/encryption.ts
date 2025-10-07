import CryptoJS from "crypto-js";

const SECRET_KEY = import.meta.env.VITE_ENCRYPTION_KEY || "default_secret_key";

/**
 * Encrypt string data
 * @param data string
 * @returns encrypted string
 */
export const encryptData = (data: string): string => {
  return CryptoJS.AES.encrypt(data, SECRET_KEY).toString();
};

/**
 * Decrypt string data
 * @param encrypted string
 * @returns decrypted string
 */
export const decryptData = (encrypted: string): string => {
  try {
    const bytes = CryptoJS.AES.decrypt(encrypted, SECRET_KEY);
    return bytes.toString(CryptoJS.enc.Utf8);
  } catch (error) {
    console.error("Decryption failed", error);
    return "";
  }
};
