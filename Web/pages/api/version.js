import packageJson from "../../package.json";
import handleRequest from "../../utils/api-mediator";

const DEFINITION = {
	GET: {
		onRequest: onGetRequest,
	},
};

export default async function version(req, res) {
	await handleRequest(req, res, DEFINITION);
}

async function onGetRequest() {
	return {
		"web-version": packageJson.version,
		"studio-version": "21.4.0",
		release: "alpha",
	};
}
