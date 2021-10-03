import Link from "next/link";

export default function notFoundPage() {
	return (
		<div className="w-full h-full flex justify-center items-center text-gray-700">
			<div className="flex flex-col gap-y-4">
				<h4 className="text-4xl">404 | Not Found</h4>

				<Link href="/">
					<a className="px-3 py-1 text-center font-medium rounded-lg text-blue-500 text-lg transition duration-200 ease-in-out hover:bg-blue-50">
						Go back to the home page
					</a>
				</Link>
			</div>
		</div>
	);
}
