export type Monster = {
    ailments: any[],
    description: string,
    elements: string[],
    id: number,
    locations: Location[],
    name: string,
    resistances: Resistance[],
    rewards: Reward[],
    species: string,
    type: string,
    weaknesses: Weakness[]
}

export type Location = {
    id: number,
    name: string,
    zoneCount: number
}

export type Resistance = {
    condition: string | null,
    element: string
}

export type Reward = {
    conditions: Condition[],
    id: number,
    item: Item
}

export type Weakness = {
    condition: string | null,
    element: string,
    stars: number
}

export type Condition = {
    chance: number,
    quantity: number,
    rank: string,
    subtype: string | null,
    type: string
}

export type Item = {
    carryLimit: number,
    description: string,
    id: number,
    name: string,
    rarity: number,
    value: number
}