import { useEffect } from "react";
import Layout from "../components/Layout";
import "tailwindcss/tailwind.css";
import { updagePageViews } from "../client/update-page-views";

export default function slstudioApp({ Component, pageProps, router }) {
	useEffect(() => {
		try {
			updagePageViews(router.asPath);
		} catch {}
	});

	return (
		<Layout>
			<Component {...pageProps} />
		</Layout>
	);
}
