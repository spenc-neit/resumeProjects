import { Text, StyleSheet, SafeAreaView, View, Image } from "react-native";
import { useAsyncStorage } from "@react-native-async-storage/async-storage";
import { useFocusEffect } from "@react-navigation/native";
import React, { useState } from "react";
import { Armor } from "./types/ArmorTypes";
import { material } from "./types/MaterialListItem";
import { addArmorMaterialsToList } from "./functions/addArmorMaterialsToList";

type asyncItem = {
	head: Armor;
	chest: Armor;
	gloves: Armor;
	waist: Armor;
	legs: Armor;
};

export const MaterialsList = () => {
	const { getItem, setItem } = useAsyncStorage("@storage_key");
	const [armorList, setArmorList] = useState<asyncItem | null>();
	useFocusEffect(
		React.useCallback(() => {
			retrieveArmor();
			console.log("grabbed from async");
		}, [])
	);

	const retrieveArmor = async () => {
		// console.log("retrieveFavorites");
		const armor = await getItem();

		if (armor !== null) {
			setArmorList(JSON.parse(armor));
			//put in if statement to satisfy typescript
		}
	};

	if (armorList === null || armorList === undefined) {
		return (
			<View style={styles.emptyArmorView}>
				<Text style={styles.subHeaderEmptyArmor}>When 5 armor pieces are chosen in the armor builder, the materials needed to craft the armor set appear here.</Text>
			</View>
		);
	}

	const head = armorList.head;
	const chest = armorList.chest;
	const gloves = armorList.gloves;
	const waist = armorList.waist;
	const legs = armorList.legs;

	let materials: material[] = [];

	materials = addArmorMaterialsToList(materials, head);
	materials = addArmorMaterialsToList(materials, chest);
	materials = addArmorMaterialsToList(materials, gloves);
	materials = addArmorMaterialsToList(materials, waist);
	materials = addArmorMaterialsToList(materials, legs);

	console.log("armorList", armorList);

	console.log("THIS SHOULD ONLY BE HERE WHEN ARMORLIST HAS VALUES IN IT.");

	return (
		<View style={styles.safeAreaView}>
			<Text style={styles.subHeader}>When 5 armor pieces are chosen in the armor builder, the materials needed to craft the armor set appear here.</Text>
			{materials.map((element)=>{
                return(<Text key={element.name} style={styles.materialItem}>
                    {element.name}, x{element.quantity}
                </Text>)
            })}
		</View>
	);
};

const styles = StyleSheet.create({
	safeAreaView: {
		flex: 1,
        backgroundColor: "#fcf5e6",
        padding:10,
		// paddingTop: Platform.OS === "android" ? StatusBar.currentHeight : 0,
	},
    loadingImage:{
        width:100,
        height:100
    },
    emptyArmorView:{
        flex:1,
        alignItems:"center",
        justifyContent:"center",
        backgroundColor: "#fcf5e6"
    }, materialItem:{
        fontSize:18,
        marginVertical:5
    },
	subHeader:{
		fontSize: 20,
		textAlign:"center",
		color:"darkgray"
	},
	subHeaderEmptyArmor:{
		fontSize: 30,
		textAlign:"center",
		color:"darkgray",
		width:"80%"
	}
});
