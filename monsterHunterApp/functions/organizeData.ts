import { Armor } from "../types/ArmorTypes";
import { DropdownItem } from "../types/DropdownItem";


export const organizeData = (data: any) => {
    let head: DropdownItem[] = [];
    let chest: DropdownItem[] = [];
    let gloves: DropdownItem[] = [];
    let waist: DropdownItem[] = [];
    let legs: DropdownItem[] = [];

    let i: number = 0;

    data.forEach((element: Armor) => {
        const item = {label: element.name, value: `${i}`}
        switch(element.type){
            case "head":
                head.push(item)
                break;
            case "chest":
                chest.push(item)
                break;
            case "gloves":
                gloves.push(item)
                break
            case "waist":
                waist.push(item)
                break
            case "legs":
                legs.push(item)
                break
        }
        i++;
    });

    return {head, chest, gloves, waist, legs}
}