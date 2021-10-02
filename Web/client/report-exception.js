import { post } from "../utils/http-methods";

const BASE_ROUTE = "report-exception";

export async function reportException(exception) {
	exception = `[WEB] - ${exception}`;
	return await post(BASE_ROUTE, null, { exception });
}
