import Link from "next/link";
import { FaDiscord, FaGithub } from "react-icons/fa";

export default Contacts;

function Contacts() {
	return (
		<div className="flex space-x-2">
			<Link href="https://discord.gg/gw8S6xT8qS">
				<a>
					<FaDiscord
						size={32}
						className="opacity-70 transition duration-200 ease-in-out hover:opacity-100"
					/>
				</a>
			</Link>

			<Link href="https://github.com/adnan-54/SLStudio">
				<a>
					<FaGithub
						size={32}
						className="opacity-70 transition duration-200 ease-in-out hover:opacity-100"
					/>
				</a>
			</Link>
		</div>
	);
}
