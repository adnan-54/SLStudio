import * as yup from "yup";
import handleRequest from "../../utils/api-mediator";

const DEFINITION = {
	GET: {
		schema: yup.object().shape({
			fundamentalFrequency: yup.number().positive().required(),
			strokes: yup.number().integer().positive().required().oneOf([2, 4]),
			cylinders: yup.number().integer().positive().required(),
		}),
		onRequest: onGetRequest,
	},
};

export default async function calculateRpm(req, res) {
	await handleRequest(req, res, DEFINITION);
}

async function onGetRequest({ fundamentalFrequency, strokes, cylinders }) {
	const rpm = (30 * fundamentalFrequency * strokes) / cylinders;

	return {
		rpm,
	};
}
