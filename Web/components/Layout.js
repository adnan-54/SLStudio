import Head from "next/head";
import Footer from "../components/Footer";
import Header from "../components/Header";

export default Layout;

function Layout({ children }) {
	return (
		<div className="flex flex-col h-screen">
			<Head>
				<title>SLStudio</title>
				<link rel="icon" href="/favIcon.ico" />
				<meta name="keywords" content="SLRR, SLStudio, SLRR Mods, adnan54" />
				<meta name="description" content="SLRR modding IDE" />
				<meta name="author" content="adnan54" />
			</Head>

			<Header />

			<div className="flex-auto">{children}</div>

			<Footer />
		</div>
	);
}
