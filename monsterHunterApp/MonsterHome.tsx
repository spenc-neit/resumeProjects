import {
	Text,
	StyleSheet,
	SafeAreaView,
	View,
	FlatList,
	TouchableOpacity, Image
} from "react-native";
import { useFetch } from "./hooks/useFetch";
import { ScrollView } from "react-native-gesture-handler";
import { StackNavigationProp } from "@react-navigation/stack";
import { MonsterStackParams } from "./types/MonsterStackParams";
import { useNavigation } from "@react-navigation/native";
import { Monster } from "./types/MonsterTypes";

export const MonsterHome = () => {
	const { data, loading, error } = useFetch("https://mhw-db.com/monsters");

	type MonsterListNav = StackNavigationProp<MonsterStackParams, "MonsterHome">;
	const navigator = useNavigation<MonsterListNav>();

	const handleItemPressed = (id: string) => {
		navigator.navigate("MonsterDetail", { monsterID: id });
	};

	if (loading) {
		return (
			<View style={styles.loadingView}>
					<Image
						source={{
							uri: "https://i.gifer.com/ZKZg.gif",
						}}
						alt={"loading"}
						style={styles.loadingImage}
					/>
			</View>
		);
	} else if (error || data == null) {
		return (
			<View style={styles.safeAreaView}>
				<Text>Error fetching data</Text>
			</View>
		);
	}

	const listOfMonsters: Monster[] = data.data.slice(16);
	console.log(listOfMonsters);

    listOfMonsters.forEach(element => {
        console.log(element.rewards)
    });

	return (
		<View style={styles.safeAreaView}>
			{/* <Text>This is monster home</Text> */}
			<ScrollView
				style={styles.monsterScrollViewExternal}
				contentContainerStyle={styles.monsterScrollViewInternal}
			>
				{listOfMonsters.map((monster: Monster, index: number) => (
					<TouchableOpacity
						key={index}
						style={styles.monsterCard}
						onPress={() => {
							handleItemPressed(String(monster.id));
						}}
					>
						<Text>{monster.name}</Text>
					</TouchableOpacity>
				))}
			</ScrollView>
		</View>
	);
};

const styles = StyleSheet.create({
	safeAreaView: {
		flex: 1,
		alignItems: "center",
		width: "100%",
        backgroundColor: "#fcf5e6",
        padding:10
		// paddingTop: Platform.OS === "android" ? StatusBar.currentHeight : 0,
	},
	monsterCard: {
		height: 50,
		borderWidth: 1,
		borderRadius: 10,
		alignItems: "center",
		justifyContent: "center",
		marginVertical: 5,
		width: "75%",
		backgroundColor:"white"
	},
	monsterScrollViewExternal: {
		width: "100%",
	},
	monsterScrollViewInternal: {
		alignItems: "center",
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
