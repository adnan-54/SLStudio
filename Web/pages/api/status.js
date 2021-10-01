import handleRequest from "../../utils/api-mediator";

const DEFINITION = {
	GET: {
		onRequest: onGetRequest,
	},
};

export default async function status(req, res) {
	await handleRequest(req, res, DEFINITION);
}

async function onGetRequest({ req }) {
	return {
		message: "OK",
		headers: req.headers,
		cookies: req.cookies,
		query: req.query,
		body: req.body,
	};
}
