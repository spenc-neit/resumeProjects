import { useState, useEffect } from "react";
import axios, { AxiosResponse } from "axios";

export const useFetch = (url: string) => {
	const [data, setData] = useState<AxiosResponse | null>(null);
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState(undefined);

	let checkedUrl = url;

	// console.log("url", url)
	// console.log("typeof", typeof(url))
	// console.log("length", url.length)

	if(url.length < 3){
		checkedUrl = `https://mhw-db.com/monsters/${url}`
	}

	console.log("checkedurl", checkedUrl)

	useEffect(() => {
		const getData = async () => {
			setError(undefined);
			try {
				const response = await axios.get(`${checkedUrl}`);
				setData(response);
			} catch (error: any) {
				console.log(error);
				setError(error);
			} finally {
				setLoading(false);
			}
		};
		getData();
	}, []);
	return { data, loading, error };
};
