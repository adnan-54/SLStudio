import supabaseClient from "../../utils/supabase-client";
import handleRequest from "../../utils/api-mediator";

const DEFINITION = {
	POST: {
		onRequest: onPostRequest,
	},
};

export default async function updatePageViews(req, res) {
	await handleRequest(req, res, DEFINITION);
}

async function onPostRequest({ req }) {
	const page = req.query.page;
	return await supabaseClient.rpc("update_page_views", {
		url: page,
	});
}
