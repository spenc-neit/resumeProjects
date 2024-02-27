import { Armor } from "../types/ArmorTypes";
import { StyleSheet } from "react-native";

export type Props = {
	props: {
		head: Armor;
		chest: Armor;
		gloves: Armor;
		waist: Armor;
		legs: Armor;
	};
};

import { View, Text, ScrollView } from "react-native";
import { SkillListItem } from "../types/SkillListItem";
import { addSkillsToList } from "../functions/addSkillsToList";
import React from "react";

export const DisplayArmorInfo = ({ props }: Props) => {
	const { head, chest, gloves, waist, legs } = props;

	console.log("head", head);

	const { def, drg, fir, ice, thu, wtr } = calculateStats([
		head,
		chest,
		gloves,
		waist,
		legs,
	]);

	let skills: SkillListItem[] = [];
	skills = addSkillsToList(skills, head);
	skills = addSkillsToList(skills, chest);
	skills = addSkillsToList(skills, gloves);
	skills = addSkillsToList(skills, waist);
	skills = addSkillsToList(skills, legs);

	console.log(skills);

	const printSkills = (skills: SkillListItem[]) => {
		let i = 0;
		return skills.map((skill, index) => {
			return (
				<React.Fragment key={index}>
					<Text style={styles.skillItem}>
						{skill.name}, Level {skill.level}
					</Text>
					<Text style={styles.skillDescription}>{skill.description}</Text>
				</React.Fragment>
			);
		});
	};

	return (
		<View>
			<ScrollView style={styles.armorScrollView}>
				<Text style={styles.subHeader}>Defense: {def}</Text>
				<View>
					<Text style={styles.subHeader}>Resistances:</Text>
					<Text style={styles.bulletItem}>Dragon: {drg}</Text>
					<Text style={styles.bulletItem}>Fire: {fir}</Text>
					<Text style={styles.bulletItem}>Ice: {ice}</Text>
					<Text style={styles.bulletItem}>Thunder: {thu}</Text>
					<Text style={styles.bulletItem}>Water: {wtr}</Text>
				</View>
				<Text style={styles.subHeader}>Skills:</Text>
				{printSkills(skills)}
			</ScrollView>
		</View>
	);
};

const calculateStats = (armors: Armor[]) => {
	let def = 0;
	let drg = 0;
	let fir = 0;
	let ice = 0;
	let thu = 0;
	let wtr = 0;
	armors.forEach((armor) => {
		def += armor.defense.base;
		drg += armor.resistances.dragon;
		fir += armor.resistances.fire;
		ice += armor.resistances.ice;
		thu += armor.resistances.thunder;
		wtr += armor.resistances.water;
	});
	return { def, drg, fir, ice, thu, wtr };
};

const styles = StyleSheet.create({
	bulletItem: {
		paddingLeft: 15,
	},
	skillItem: {},
	skillDescription: {
		fontSize: 10,
	},
	armorScrollView: { height: 400, paddingTop: 5 },
	subHeader: {
		fontSize: 20,
		marginVertical:5
	}
});
