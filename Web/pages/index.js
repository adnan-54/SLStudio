import Link from "next/link";

export default function home() {
	return (
		<div className="flex flex-col items-center w-full h-full text-gray-800 my-10 gap-y-10">
			<div className="container flex flex-col mt-20 items-center text-center space-y-14 px-5">
				<h1 className="text-5xl font-extrabold md:text-6xl lg:text-8xl">
					The Modding IDE for SLRR
				</h1>

				<h4 className="text-xl">
					SLStudio gives you the best developer experience with all the features
					you need for modding: friendly & flexible interface, multiple files
					format support, advanced editors, error checking, and more.
				</h4>

				<div className="mt-5 flex flex-col w-1/3 gap-y-4 gap-x-16 text-lg font-medium md:w-1/2 md:flex-row">
					<Link href="/news">
						<a className="flex-1 border shadow px-4 py-4 rounded-md transition duration-200 ease-in-out  hover:shadow-lg">
							News
						</a>
					</Link>

					<Link href="/downloads">
						<a className="flex-1 border text-white shadow bg-blue-600 px-4 py-4 rounded-md transition duration-200 ease-in-out hover:bg-blue-500 hover:shadow-lg">
							Download Now
						</a>
					</Link>
				</div>
			</div>

			<div className="container mt-20 flex flex-col items-center w-full gap-y-3 px-5">
				<h2 className="text-3xl font-bold">Why SLStudio</h2>

				<p>The best modders use and love SLStudio</p>

				<div className="mt-8 flex flex-wrap justify-center gap-6">
					<div className="flex flex-col p-6 w-80 h-40 gap-y-4 border rounded transition duration-200 ease-in-out hover:shadow-lg">
						<h4 className="text-xl font-semibold">Modern Interface</h4>

						<p className="text-sm">
							Modern, friendly, and flexible interface inspired by the popular
							IDE Visual Studio.
						</p>
					</div>

					<div className="flex flex-col p-6 w-80 h-40 gap-y-4 border rounded transition duration-200 ease-in-out hover:shadow-lg">
						<h4 className="text-xl font-semibold">Multiple Files Support</h4>

						<p className="text-sm">
							Directly handle multiple of the file formats used by the game. No
							need for conversions anymore!
						</p>
					</div>

					<div className="flex flex-col p-6 w-80 h-40 gap-y-4 border rounded transition duration-200 ease-in-out hover:shadow-lg">
						<h4 className="text-xl font-semibold">Advanced Editors</h4>

						<p className="text-sm">
							File editors with advanced features like syntax highlighting,
							autocomplete suggestions, and many more.
						</p>
					</div>

					<div className="flex flex-col p-6 w-80 h-40 gap-y-4 border rounded transition duration-200 ease-in-out hover:shadow-lg">
						<h4 className="text-xl font-semibold">Error Checking</h4>

						<p className="text-sm">
							Find and fix errors quickly. Test your mod and make sure it is up
							to the highest standards.
						</p>
					</div>

					<div className="flex flex-col p-6 w-80 h-40 gap-y-4 border rounded transition duration-200 ease-in-out hover:shadow-lg">
						<h4 className="text-xl font-semibold">Multiple Languages</h4>

						<p className="text-sm">
							English and Brazilian Portuguese support, but more languages can
							be easily added by the community.
						</p>
					</div>

					<div className="flex flex-col p-6 w-80 h-40 gap-y-4 border rounded transition duration-200 ease-in-out hover:shadow-lg">
						<h4 className="text-xl font-semibold">Multiple Themes</h4>

						<p className="text-sm">
							Choose between two themes: Light and Dark. More themes can also be
							added by the community.
						</p>
					</div>
				</div>
			</div>
		</div>
	);
}
