export type Armor = {
    armorSet: ArmorSet,
    assets: Assets,
    attributes: any, //is blank and being phased out by the API devs
    crafting: Crafting,
    defense: Defense,
    id: number,
    name: string,
    rank: string,
    rarity: number,
    resistances: Resistance,
    skills: Skill[],
    slots: Slot[],
    type: string
}

type ArmorSet = {
    bonus: number,
    id: number,
    name: string,
    pieces: number[],
    rank: string
}

type Assets = {
    imageFemale: string,
    imageMale: string
}

type Crafting = {
    materials: Material[]
}

type Material = {
    item: Item,
    quantity: number
}

type Item = {
    carryLimit: number,
    description: string,
    id: number,
    name: string,
    rarity: number,
    value: number
}

type Defense = {
    augmented: number,
    base: number,
    max: number
}

type Resistance = {
    dragon: number,
    fire: number,
    ice: number,
    thunder: number,
    water: number
}

type Skill = {
    description: string,
    id: number,
    level: number,
    modifiers: Object[],
    skill: number,
    skillName: string
}

type Slot = {
    rank: number
}