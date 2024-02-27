import { createStackNavigator } from "@react-navigation/stack";
import { MonsterStackParams } from "./types/MonsterStackParams";
import { MonsterHome } from "./MonsterHome";
import { MonsterDetail } from "./MonsterDetail";
import {Text, StyleSheet, View} from 'react-native'

export const MonsterStackNavigator = () => {
    const MonsterStack = createStackNavigator<MonsterStackParams>();
    return(
        <MonsterStack.Navigator screenOptions={{headerShown:false}}>
            <MonsterStack.Screen name="MonsterHome" component={MonsterHome}/>
            <MonsterStack.Screen name="MonsterDetail" component={MonsterDetail} />
        </MonsterStack.Navigator>
    )
}