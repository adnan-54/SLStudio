import supabaseClient from "./supabase-client";

export default async function handleRequest(request, response, definition) {
	try {
		const validationResult = await validate(request, definition);
		if (!validationResult.isValid) {
			await sendResponse(
				request,
				response,
				validationResult.statusMessage,
				validationResult.statusCode,
				validationResult.statusMessage
			);
			return;
		}

		const onRequest = getOnRequest(request.method, definition);
		const result = await onRequest(validationResult.data);

		await sendResponse(
			request,
			response,
			validationResult.statusMessage,
			validationResult.statusCode,
			result
		);
	} catch (error) {
		await sendResponse(request, response, error, 500);
		throw error;
	}
}

async function validate(request, definition) {
	const requestValidation = validateRequest(request);
	if (!requestValidation.isValid) return requestValidation;

	const definitionValidation = validateDefinition(request.method, definition);
	if (!definitionValidation.isValid) return definitionValidation;

	const dataValidation = await validateData(
		request.body,
		request.method,
		definition,
		request
	);
	if (!dataValidation.isValid) return dataValidation;

	return createOkResult(dataValidation.data);
}

function validateRequest(request) {
	const clientId = request.headers.clientid?.toUpperCase();

	if (!clientId)
		return createValidationResult(false, 400, "'clientid' header is required");

	if (clientId !== "53EA78AB-6930-4ED7-8349-CCD3478F2F99")
		return createValidationResult(false, 400, "Invalid clientid");

	return createOkResult();
}

function validateDefinition(method, definition) {
	function hasMethod() {
		return (
			definition.GET ||
			definition.POST ||
			definition.PUT ||
			definition.DELETE ||
			definition.PATCH
		);
	}

	if (!hasMethod())
		return createValidationResult(
			false,
			500,
			"At least one method is required"
		);

	function supportsMethod() {
		return definition[method];
	}

	if (!supportsMethod())
		return createValidationResult(
			false,
			405,
			`Method ${method} is not supported`
		);

	function hasOnRequest() {
		return definition[method].onRequest;
	}

	if (!hasOnRequest())
		return createValidationResult(
			false,
			500,
			"The function 'onRequest' is required"
		);

	function requiresAuthentication() {
		return definition[method].requiresAuthentication;
	}

	if (requiresAuthentication())
		return createValidationResult(
			false,
			501,
			"Authentication is not implemented yet"
		);

	return createOkResult();
}

async function validateData(data, method, definition, request) {
	const schema = definition[method].schema;

	let result = {};

	if (!data) data = {};

	if (schema) {
		try {
			result = await schema.validate(data);
		} catch ({ errors }) {
			return createValidationResult(false, 400, errors[0]);
		}
	}

	result.req = request;

	return createOkResult(result);
}

function createValidationResult(isValid, statusCode, statusMessage, data) {
	return { isValid, statusCode, statusMessage, data };
}

function createOkResult(data) {
	if (!data) data = {};

	return createValidationResult(true, 200, "OK", data);
}

async function sendResponse(
	request,
	response,
	statusMessage,
	statusCode,
	data
) {
	await pushToHistory(request, statusCode, statusMessage);

	response.status(statusCode);

	if (!data) response.send();
	else response.json(data);
}

async function pushToHistory(request, statusCode, statusMessage) {
	const method = request.method;
	const route = request.url;

	const data = {
		method,
		route,
		status_code: statusCode,
		status_message: statusMessage,
	};

	await supabaseClient.from("api_history").insert([data]);
}

function getOnRequest(method, definition) {
	return definition[method].onRequest;
}
