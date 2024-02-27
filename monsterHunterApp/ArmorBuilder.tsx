import { Text, StyleSheet, SafeAreaView, View, Image } from "react-native";
import { useEffect, useState } from "react";
import { useFetch } from "./hooks/useFetch";
import { organizeData } from "./functions/organizeData";
import { ScrollView } from "react-native-gesture-handler";
import { Dropdown } from "react-native-element-dropdown";
import { DropdownItem } from "./types/DropdownItem";
import { DisplayArmorInfo } from "./components/DisplayArmorInfo";
import { useAsyncStorage } from "@react-native-async-storage/async-storage";

export const ArmorBuilder = () => {
	const [chosenHead, setChosenHead] = useState<DropdownItem | null>();
	const [chosenChest, setChosenChest] = useState<DropdownItem | null>();
	const [chosenGloves, setChosenGloves] = useState<DropdownItem | null>();
	const [chosenWaist, setChosenWaist] = useState<DropdownItem | null>();
	const [chosenLegs, setChosenLegs] = useState<DropdownItem | null>();

	const { getItem, setItem } = useAsyncStorage("@storage_key");

	const { data, loading, error } = useFetch("https://mhw-db.com/armor");

    useEffect(() => {
		if (allChosenNotNull) {
			let item: any = {
				head: data.data[chosenHead.value],
				chest: data.data[chosenChest.value],
				gloves: data.data[chosenGloves.value],
				waist: data.data[chosenWaist.value],
				legs: data.data[chosenLegs.value],
			};
			item = JSON.stringify(item);
			setItem(item);
            console.log("wrote to async hopefully")
		}
	}, [chosenHead, chosenChest, chosenGloves, chosenWaist, chosenLegs]);

	if (loading || data === null) {
		return (
			<View style={styles.loadingView}>
				{loading ? (
					<Image
						source={{
							uri: "https://i.gifer.com/ZKZg.gif",
						}}
						alt={"loading"}
						style={styles.loadingImage}
					/>
				) : null}
			</View>
		);
	}

	const { head, chest, gloves, waist, legs } = organizeData(data.data);

	const allChosenNotNull =
		chosenHead !== undefined &&
		chosenChest !== undefined &&
		chosenGloves !== undefined &&
		chosenWaist !== undefined &&
		chosenLegs !== undefined;

	return (
		<View style={styles.safeAreaView}>
			<View style={styles.dropdowns}>
				<Dropdown
					style={styles.dropdown}
					data={head}
					search
					onChange={(item) => {
						console.log("item", item);
						setChosenHead(item);
					}}
					searchPlaceholder="Search..."
					labelField={"label"}
					valueField={"value"}
					value={chosenHead}
				/>
				<Dropdown
					style={styles.dropdown}
					data={chest}
					search
					onChange={(item) => {
						console.log("item", item);
						console.log("item in data", data.data[item.value]);
						console.log("attributes", data.data[item.value].attributes);
						setChosenChest(item);
					}}
					searchPlaceholder="Search..."
					labelField={"label"}
					valueField={"value"}
					value={chosenChest}
				/>
				<Dropdown
					style={styles.dropdown}
					data={gloves}
					search
					onChange={(item) => {
						console.log("item", item);
						setChosenGloves(item);
					}}
					searchPlaceholder="Search..."
					labelField={"label"}
					valueField={"value"}
					value={chosenGloves}
				/>
				<Dropdown
					style={styles.dropdown}
					data={waist}
					search
					onChange={(item) => {
						console.log("item", item);
						setChosenWaist(item);
					}}
					searchPlaceholder="Search..."
					labelField={"label"}
					valueField={"value"}
					value={chosenWaist}
				/>
				<Dropdown
					style={styles.dropdown}
					data={legs}
					search
					onChange={(item) => {
						console.log("item", item);
						setChosenLegs(item);
					}}
					searchPlaceholder="Search..."
					labelField={"label"}
					valueField={"value"}
					value={chosenLegs}
				/>
			</View>

			{allChosenNotNull ? ( //I'm literally checking to see if they are null before executing. I don't know what to do to satisfy typescript here.
				<DisplayArmorInfo
					props={{
						head: data.data[chosenHead.value],
						chest: data.data[chosenChest.value],
						gloves: data.data[chosenGloves.value],
						waist: data.data[chosenWaist.value],
						legs: data.data[chosenLegs.value],
					}}
				/>
			) : null}
		</View>
	);
};

const styles = StyleSheet.create({
	safeAreaView: {
		flex: 1,
		// paddingTop: Platform.OS === "android" ? StatusBar.currentHeight : 0,
        backgroundColor: "#fcf5e6",
        padding: 10,
        justifyContent:"flex-start"
	},
	dropdowns: {
		// flex: 1,
		width: "100%",
		// alignItems: "flex-start",
        // backgroundColor:"white"
	},
	dropdown: {
		width: "100%",
		borderWidth: 1,
		marginVertical: 5,
		paddingHorizontal: 5,
		borderRadius: 10,
        backgroundColor: "white"
	},
    loadingImage:{
        width:100,
        height:100
    },
    loadingView:{
        flex:1,
        alignItems:"center",
        justifyContent:"center",
        backgroundColor:"#fcf5e6"
    }
});
