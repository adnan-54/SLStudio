export async function get(route, query, data) {
	const response = await makeRequest(route, query, "GET", data);
	return processResponse(response);
}

export async function post(route, query, data) {
	const response = await makeRequest(route, query, "POST", data);
	return processResponse(response);
}

export async function put(route, query, data) {
	const response = await makeRequest(route, query, "PUT", data);
	return processResponse(response);
}

export async function del(route, query, data) {
	const response = await makeRequest(route, query, "DELETE", data);
	return processResponse(response);
}

export async function patch(route, query, data) {
	const response = await makeRequest(route, query, "PATCH", data);
	return processResponse(response);
}

function makeRequest(route, query, method, data) {
	route = buildUrl(route, query);
	const options = {
		method,
		headers: createHeaders(data),
		body: createBody(data),
	};

	return fetch(route, options);
}

function buildUrl(endPoint, query) {
	let baseUrl = `/api/${endPoint}`;

	if (!query) return baseUrl;

	const searchParams = new URLSearchParams(query);
	return `${baseUrl}?${searchParams}`;
}

function createHeaders(data) {
	const headers = {
		clientId: "53EA78AB-6930-4ED7-8349-CCD3478F2F99",
	};

	if (data) headers["Content-Type"] = "application/json";

	return new Headers(headers);
}

function createBody(data) {
	if (data) return JSON.stringify(data);
	return "";
}

function processResponse(response) {
	if (!response.ok) throw response;

	const responseType = response.headers.get("Content-Type");
	if (responseType && responseType.includes("application/json"))
		return response.json();
	return response;
}
