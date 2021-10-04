import Link from "next/link";
import Contacts from "./Contacts";

export default Footer;

function Footer() {
	return (
		<div className="text-gray-700 border-t bg-gray-100 p-5 w-full border-gray-300">
			<div className="flex flex-col items-center">
				<Contacts />

				<hr className="m-5 w-3/4 border-gray-300" />

				<p className="text-xs">
					Copyright &copy; {new Date().getFullYear()} adnan54. All rights
					reserved.
				</p>
			</div>
		</div>
	);
}
