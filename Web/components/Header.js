import Link from "next/link";

export default Header;

function Header() {
	return (
		<div>
			<div className="md:h-20" />

			<header className="py-4 w-full flex flex-col items-center shadow bg-gray-50 border-b border-gray-300 text-gray-700 md:h-20 md:flex-row md:fixed md:top-0 md:left-0 md:z-10">
				<div className="container flex flex-col items-center w-full px-3 md:flex-row md:mx-auto md:items-baseline">
					<div className="flex flex-col items-baseline text-blue-500 md:flex-row md:gap-x-1">
						<Link href="/">
							<a className="text-4xl font-black md:text-5xl">SLStudio</a>
						</Link>

						<h6 className="ml-auto">alpha</h6>
					</div>

					<div className="flex flex-col items-center my-2 font-semibold text-lg gap-y-1 gap-x-2 md:flex-row md:justify-between md:w-1/2 md:ml-auto md:items-end">
						<Link href="/">
							<a className="opacity-70 px-3 py-1 transition duration-200 ease-in-out hover:opacity-100">
								Home
							</a>
						</Link>

						<Link href="/news">
							<a className="opacity-70 px-3 py-1 transition duration-200 ease-in-out hover:opacity-100">
								News
							</a>
						</Link>

						<Link href="/help">
							<a className="opacity-70 px-3 py-1 transition duration-200 ease-in-out hover:opacity-100">
								Help
							</a>
						</Link>

						<Link href="/downloads">
							<a className="text-white mt-4 bg-blue-500 border-blue-500 border px-4 py-1 rounded-md transition duration-200 ease-in-out hover:bg-transparent hover:text-blue-500 md:mt-0 md:ml-6">
								Downloads
							</a>
						</Link>
					</div>
				</div>
			</header>
		</div>
	);
}
