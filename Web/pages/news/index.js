import Link from "next/link";
import Primsic from "prismic-javascript";
import { RichText } from "prismic-reactjs";
import prismicClient from "../../utils/prismic-client";

export default function newsPage(props) {
	return (
		<div className="flex flex-col items-center space-y-8 my-12">
			<div className="container flex flex-col items-center space-y-5">
				<h1 className="font-bold text-4xl text-gray-700">News</h1>
				<h4 className="font-normal text-gray-500">
					The latest news about SLStudio
				</h4>
			</div>

			<div className="w-full h-full flex flex-col border-b border-gray-300">
				{props.posts.map((news) => createPost(news))}
			</div>
		</div>
	);
}

export async function getStaticProps() {
	const posts = await getAllPosts();

	return {
		props: {
			posts: posts,
		},
	};
}

async function getAllPosts() {
	const posts = [];
	const response = await getResponse();
	const totalPages = response.total_pages;

	for (let pageNumber = 1; pageNumber <= totalPages; pageNumber++) {
		const page = await getPage(pageNumber);
		page.forEach((result) => {
			posts.push(result);
		});
	}

	return posts.reverse();
}

async function getPage(pageNumber) {
	const response = await getResponse(pageNumber);

	return response.results;
}

async function getResponse(pageNumber = null) {
	const response = await prismicClient.query(
		Primsic.Predicates.at("document.type", "news"),
		{ pageSize: 100, page: pageNumber ? pageNumber : 1 }
	);

	return response;
}

function createPost(news) {
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
		<div
			className="border-t border-gray-300 flex flex-col items-center transition-shadow duration-200 ease-in-out hover:shadow-lg"
			key={news.uid}>
			<div className="container max-w-3xl p-5 flex flex-col items-start">
				<Link href={`/news/${news.uid}`}>
					<a className="font-bold text-2xl text-gray-700 mt-3">
						{RichText.render(data.title)}
					</a>
				</Link>

				<h4 className="text-gray-500 font-thin">{publicationDate}</h4>

				<div className="mt-3 text-gray-600">
					{RichText.render(data.description)}
				</div>

				<Link href={`/news/${news.uid}`}>
					<a className="mt-7 -ml-3 px-3 py-1 font-medium rounded-lg text-blue-500 text-lg capitalize transition duration-200 ease-in-out hover:bg-blue-50">
						Read More →
					</a>
				</Link>
			</div>
		</div>
	);
}
