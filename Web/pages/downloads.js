import Link from "next/link";
import Primsic from "prismic-javascript";
import { RichText } from "prismic-reactjs";
import prismicClient from "../utils/prismic-client";

export default function downloadPage(props) {
	const releases = props.releases;

	return (
		<div className="flex flex-col items-center space-y-8 my-12">
			<div className="container flex flex-col items-center space-y-5">
				<h1 className="font-bold text-4xl text-gray-700">Downloads</h1>
				<h4 className="font-normal text-gray-500">
					The latest SLStudio releases
				</h4>
			</div>

			<div className="w-full h-full flex flex-col border-b border-gray-300">
				{releases.map((x) => createPost(x))}
			</div>
		</div>
	);
}

export async function getStaticProps() {
	const releases = await getAllReleases();

	return {
		props: {
			releases: releases,
		},
	};
}

async function getAllReleases() {
	const releases = [];
	const response = await getResponse();
	const totalPages = response.total_pages;

	for (let pageNumber = 1; pageNumber <= totalPages; pageNumber++) {
		const page = await getPage(pageNumber);
		page.forEach((result) => {
			releases.push(result);
		});
	}

	return releases;
}

async function getPage(pageNumber) {
	const response = await getResponse(pageNumber);

	return response.results;
}

async function getResponse(pageNumber = null) {
	const response = await prismicClient.query(
		Primsic.Predicates.at("document.type", "downloads"),
		{ pageSize: 100, page: pageNumber ? pageNumber : 1 }
	);

	return response;
}

function createPost(release) {
	const data = release.data;
	const dateOptions = {
		weekday: "long",
		year: "numeric",
		month: "long",
		day: "numeric",
	};
	const publicationDate = new Date(
		release.first_publication_date
	).toLocaleDateString("en-us", dateOptions);

	return (
		<div
			className="border-t border-gray-300 flex flex-col items-center transition-shadow duration-200 ease-in-out hover:shadow-lg"
			key={release.id}>
			<div className="container max-w-3xl p-5 flex flex-col items-start">
				<div className="font-bold text-2xl text-gray-700 mt-3">
					{RichText.render(data.release_title)}
				</div>

				<h4 className="text-gray-500 font-thin">{publicationDate}</h4>

				<h3 className="mt-2 font-medium text-lg">Release Notes:</h3>

				<div className="mt-1 ml-3.5 text-gray-600">
					{RichText.render(data.release_notes)}
				</div>

				<div
					className="mt-6 flex flex-col gap-y-4 gap-x-16 text-center w-1/3 self-center
					md:flex-row">
					{getDownloadButton(data)}
				</div>
			</div>
		</div>
	);
}

function getDownloadButton(data) {
	let available = data.download_available;

	if (available) return createDownloadButton(data);
	return createUnavailableMessage();
}

function createDownloadButton(data) {
	return (
		<Link href={data.download_link.url}>
			<a className="flex-1 font-semibold text-lg text-white bg-blue-500 border-blue-500 border px-4 py-3 rounded-md transition duration-200 ease-in-out hover:bg-transparent hover:text-blue-500">
				Download Portable
			</a>
		</Link>
	);
}

function createUnavailableMessage() {
	return (
		<div className="flex-1 text-lg font-medium opacity-50">
			<h4>Download Unavailable</h4>
		</div>
	);
}
