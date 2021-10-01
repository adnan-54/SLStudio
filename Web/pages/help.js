import Link from "next/link";

export default function helpPage() {
	return (
		<div className="flex flex-col items-center space-y-8 my-12">
			<div className="container flex flex-col items-center space-y-5">
				<h1 className="font-bold text-4xl text-gray-700">Help</h1>
				<h4 className="font-normal text-gray-500">
					The help section about SLStudio
				</h4>
			</div>

			<div className="w-full h-full flex flex-col border-b border-gray-300">
				<div className="border-t border-gray-300 flex flex-col items-center transition-shadow duration-200 ease-in-out hover:shadow-lg">
					<div className="container max-w-3xl p-5 flex flex-col items-start">
						<h1 className="font-bold text-2xl text-gray-700 mt-3">
							Discord Server
						</h1>

						<div className="mt-4 text-gray-600">
							If you have any question, comment or feedback, please feel free to
							contact us in our Discord server.
						</div>

						<Link href="https://discord.gg/gw8S6xT8qS">
							<a
								className="mt-7 -ml-3 px-3 py-1 font-medium rounded-lg text-blue-500 text-lg transition duration-200 ease-in-out hover:bg-blue-50"
								target="_blank">
								Join our Discord server →
							</a>
						</Link>
					</div>
				</div>
			</div>
		</div>
	);
}
