import { Armor } from "../types/ArmorTypes";
import { material } from "../types/MaterialListItem";

export const addArmorMaterialsToList = (materials: material[], armorPiece: Armor) => {
    armorPiece.crafting.materials.forEach(element => {
        let materialIndex = -1;


        for(let i = 0; i<materials.length; i++){
            if(materials[i].name === element.item.name){
                materialIndex = i;
            }
        }

        if(materialIndex === -1){
            materials.push({name: element.item.name, quantity: element.quantity})
        } else {
            materials[materialIndex].quantity += element.quantity
        }

    });

    return materials
}