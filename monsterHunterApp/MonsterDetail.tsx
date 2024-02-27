import { useRoute } from "@react-navigation/native";
import { Text, StyleSheet, SafeAreaView, View, ScrollView } from "react-native";
import { useFetch } from "./hooks/useFetch";
import { RouteProp } from "@react-navigation/native";
import { MonsterStackParams } from "./types/MonsterStackParams";
import { Location, Monster, Reward, Weakness } from "./types/MonsterTypes";
import { titleCase } from "./functions/titleCase";
import { sentenceCase } from "./functions/sentenceCase";

type MonsterDetailParams = RouteProp<MonsterStackParams, "MonsterDetail">;

export const MonsterDetail = () => {
	const { params } = useRoute<MonsterDetailParams>();

	const { monsterID } = params;

	if (params === null) {
		return <Text>params is null</Text>;
	}

	let id: string = String(monsterID);

	const { data, loading, error } = useFetch(id);

	if (loading) {
		return <Text>Loading...</Text>;
	}
	if (error) {
		return <Text>error</Text>;
	}
	if (data === null) {
		return <Text>data is null</Text>;
	}

	const monster: Monster = data.data;

	console.log(monster);

	const elementPrint = (elements: string[]) => {
		if (elements.length === 0) {
			return "This monster does not attack with elements.";
		} else if (elements.length === 1) {
			return `This monster attacks with the ${elements[0]} element.`;
		} else {
			let elementFirst = true;
			let elementLast = false;
			let outputString = "This monster attacks with the ";
			for (let i = 0; i < elements.length; i++) {
				if (i + 2 > elements.length) {
					elementLast = true;
				}
				if (elementFirst) {
					outputString += elements[i];
					elementFirst = false;
				} else if (elementLast) {
					outputString += ` and ${elements[i]} elements.`;
				} else {
					outputString += `, ${elements[i]} `;
				}
			}
			return outputString;
		}
	};

	const weaknessPrint = (weaknesses: Weakness[]) => {
		let i: number = 0;
		return weaknesses.map((weakness) => {
			return (
				<Text key={i++}>
					{sentenceCase(weakness.element)}: {weakness.stars} stars
				</Text>
			);
		});
	};

	const locationsPrint = (locations: Location[]) => {
		let i: number = 0;
		return locations.map((location)=>{
			return (
				<Text key={i++}>
					{location.name} in zone {location.zoneCount}
				</Text>
			)
		});
	};

	return (
		<View style={styles.safeAreaView}>
			<Text style={styles.monsterName}>{monster.name}</Text>
			<Text style={styles.monsterClassification}>{titleCase(monster.species)}</Text>
			<Text style={styles.description}>{monster.description}</Text>
			<Text style={styles.element}>{elementPrint(monster.elements)}</Text>
			<View style={styles.sideBySide}>
				<View style={styles.sbsView}>
					<Text style={styles.subHeader}>Weaknesses:</Text>
					<View>{weaknessPrint(monster.weaknesses)}</View>
				</View>
				<View style={styles.sbsView}>
					<Text style={styles.subHeader}>Locations:</Text>
					<View>{locationsPrint(monster.locations)}</View>
				</View>
			</View>
		</View>
	);
};

const styles = StyleSheet.create({
	safeAreaView: {
		flex: 1,
		// paddingTop: Platform.OS === "android" ? StatusBar.currentHeight : 0,
		padding: 10,
		backgroundColor:"#fcf5e6"
	},
	sideBySide:{
		flex:1,
		flexDirection:"row",
		justifyContent:"space-around",
		// backgroundColor:"lightblue"
	},
	monsterName:{
		fontSize: 50,
		textAlign:"center"
	},
	monsterClassification:{
		fontSize: 25,
		textAlign:"center",
		marginBottom:10
	},
	description:{
		fontStyle:"italic",
		color:"darkgray",
		fontSize: 15,
	},
	subHeader:{
		fontSize:20
	},
	element:{
		fontSize: 20,
		textAlign:"center",
		marginVertical:10
	},
	sbsView:{
		alignItems:"center"
	}

});