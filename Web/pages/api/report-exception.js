import * as yup from "yup";
import handleRequest from "../../utils/api-mediator";
import supabaseClient from "../../utils/supabase-client";

const DEFINITION = {
	POST: {
		schema: yup.object().shape({
			exception: yup.string().required(),
		}),
		onRequest: onPostRequest,
	},
};

export default async function reportException(req, res) {
	await handleRequest(req, res, DEFINITION);
}

async function onPostRequest({ exception }) {
	const { data, error } = await supabaseClient.from("exceptions").insert([
		{
			exception,
		},
	]);

	if (error) throw error;

	return data[0];
}
