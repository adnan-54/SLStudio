import Prismic from "prismic-javascript";

export const apiEndpoint = process.env.PRISMIC_ENDPOINT;
export const accessToken = process.env.PRISMIC_TOKEN;

const createClientOptions = (req = null, prismicAccessToken = null) => {
	const reqOption = req ? { req } : {};
	const accessTokenOption = prismicAccessToken
		? { accessToken: prismicAccessToken }
		: {};
	return {
		...reqOption,
		...accessTokenOption,
	};
};

const prismicClient = Prismic.client(
	apiEndpoint,
	createClientOptions(null, accessToken)
);

export default prismicClient;
