import { useEffect } from "react";
import Layout from "../components/Layout";
import "tailwindcss/tailwind.css";
import { updagePageViews } from "../client/update-page-views";
import { reportException } from "../client/report-exception";

export default function slstudioApp({ Component, pageProps, router }) {
	useEffect(() => {
		const updatePageViews = async () => {
			try {
				await updagePageViews(router.asPath);
			} catch (e) {
				await reportException(e);
			}
		};

		updatePageViews();
	});

	return (
		<Layout>
			<Component {...pageProps} />
		</Layout>
	);
}
