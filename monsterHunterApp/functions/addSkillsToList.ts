import { Armor } from "../types/ArmorTypes";
import { SkillListItem } from "../types/SkillListItem";

export const addSkillsToList = (skills: SkillListItem[], armor: Armor) => {
    armor.skills.forEach(element => {
        let skillIndex = -1

        for(let i = 0; i<skills.length; i++){
            if(skills[i].name === element.skillName){
                skillIndex = i;
            }
        }

        if(skillIndex === -1){
            skills.push({name:element.skillName, description:element.description, level:element.level})
        } else {
            skills[skillIndex].level += element.level
        }

    });

    return skills
}