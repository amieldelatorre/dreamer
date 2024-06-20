import { BaseUrl } from "./const";
import { Request, RequestArgs, SupportedMethods } from "./fetch";

const accessTokenKey = "dreamer_access_token";
const LoginBaseUrl = new URL("auth/login", BaseUrl);

export interface LoginCredentials {
  email: string
  password: string
}

export const Login = async (data: LoginCredentials) => {
  let jsonData = JSON.stringify(data);

  var requestArgs: RequestArgs = {
    url: LoginBaseUrl,
    method: SupportedMethods.POST,
    extraHeaders: {},
    body: jsonData
  }

  let result = await Request(requestArgs);
  return result;
}

export const SetAccessToken = (accessToken: string) => {
  localStorage.setItem(accessTokenKey, accessToken);
}

export const GetAccessToken = () => {
  return localStorage.getItem(accessTokenKey);
}