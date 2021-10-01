import Link from "next/link";

export default Footer;

function Footer() {
	return (
		<div className="text-gray-700 border-t bg-gray-100 p-5 w-full border-gray-300">
			<div className="flex flex-col items-center">
				<div className="flex flex-col items-center">
					<a className="font-medium">Need help?</a>
					<Link href="https://discord.gg/gw8S6xT8qS">
						<a
							className="px-3 py-1 rounded-lg text-blue-500 transition duration-200 ease-in-out hover:bg-blue-100"
							target="_blank">
							Join our Discord server
						</a>
					</Link>
				</div>

				<hr className="m-5 w-3/4 border-gray-300" />

				<p className="text-xs">
					Copyright &copy; {new Date().getFullYear()} adnan54. All rights
					reserved.
				</p>
			</div>
		</div>
	);
}
