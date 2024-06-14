import { BaseUrl } from "./const";
import { Request, RequestArgs, SupportedMethods } from "./fetch";

const UserBaseUrl = new URL("user", BaseUrl);

export interface CreateUserData {
  firstName: string
  lastName: string
  email: string
  password: string
}

export const CreateUser = async (data: CreateUserData) => {
  let jsonData = JSON.stringify(data);

  var requestArgs: RequestArgs = {
    url: UserBaseUrl,
    method: SupportedMethods.POST,
    extraHeaders: {},
    body: jsonData
  }

  let result = await Request(requestArgs);
  return result;
}