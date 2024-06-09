import {
  ResponseError,
  FetchError,
  PublicClientErrorFromJSON,
  PublicErrorFromJSON
} from "../../../generated";
import * as runtime from "../../../generated/runtime";

export async function getRequestError(reason: unknown): Promise<string> {
  if (reason instanceof ResponseError) {
    if (reason.response.status === 400) {
      const json = new runtime.JSONApiResponse(reason.response, PublicClientErrorFromJSON);
      const publicError = await json.value();

      return [publicError.description, ...(publicError.validationErrors ?? [])].join("\n");
    } else {
      const json = new runtime.JSONApiResponse(reason.response, PublicErrorFromJSON);
      const publicError = await json.value();

      return publicError.description;
    }
  } else if (reason instanceof FetchError) {
    return "Не вдалося отримати дані";
  }

  if (reason instanceof Object)
    return reason.toString();

  return "Невідома помилка";
}
