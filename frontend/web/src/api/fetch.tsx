export enum SupportedMethods {
  GET = "GET",
  POST = "POST",
  PUT = "PUT",
  DELETE = "DELETE"
}

export interface RequestArgs {
  url: URL
  method: SupportedMethods
  body?: string // Expect body to be already json stringified
  extraHeaders: {[key :string]: string}
}

export const Request = async (args: RequestArgs) => {
  let options = {
      method: args.method,
      cors: "cors",
      headers: {
        "Content-Type": "application/json",
        ...args.extraHeaders
      },
      body: args.body
  }

  let result = await fetch(args.url, options)
    .then(result => {
      return result;
    });
    // Catch is handled by the original function caller

    return result
}
