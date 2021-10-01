import { post } from "../utils/http-methods";

const BASE_ROUTE = "update-page-views";

export async function updagePageViews(pageUrl) {
	return await post(BASE_ROUTE, { pageUrl }, null);
}
