import styles from "../../styles/RichText.module.css";
import Primsic from "prismic-javascript";
import { RichText } from "prismic-reactjs";
import prismicClient from "../../utils/prismic-client";

export default function newsPage(props) {
	const news = props.news;
	const data = news.data;
	const dateOptions = {
		weekday: "long",
		year: "numeric",
		month: "long",
		day: "numeric",
	};
	const publicationDate = new Date(
		news.last_publication_date
	).toLocaleDateString("en-us", dateOptions);

	return (
		<div className="flex flex-col items-center space-y-8 my-12">
			<div className="container flex flex-col items-center space-y-5">
				<div className="font-bold text-4xl text-gray-700 text-center">
					{RichText.render(data.title)}
				</div>
				<h4 className="font-thin text-gray-500">{publicationDate}</h4>
			</div>

			<hr className="w-full border-gray-300" />

			<div className="container max-w-4xl flex flex-col mx-6 px-8 space-y-8">
				<div className={styles.content}>
					{RichText.render(data.description)}
				</div>

				<div className={styles.content}>{RichText.render(data.content)}</div>
			</div>
		</div>
	);
}

export async function getStaticPaths() {
	const paths = [];
	const response = await getResponse();
	const totalPages = response.total_pages;

	for (let i = 1; i <= totalPages; i++) {
		let page = await getPage(i);
		page.forEach((news) => {
			let path = createPath(news.uid);
			paths.push(path);
		});
	}

	return {
		paths: paths,
		fallback: false,
	};
}

export async function getStaticProps(context) {
	const news = await getByUid(context.params.uid);

	return {
		props: {
			news: news,
		},
	};
}

async function getPage(pageNumber) {
	const response = await getResponse(pageNumber);

	return response.results;
}

async function getResponse(pageNumber = null) {
	const response = await prismicClient.query(
		Primsic.Predicates.at("document.type", "news"),
		{ pageSize: 20, page: pageNumber ? pageNumber : 1 }
	);

	return response;
}

function createPath(uid) {
	return {
		params: {
			uid: uid,
		},
	};
}

async function getByUid(uid) {
	const response = await prismicClient.getByUID("news", uid);

	return response;
}
