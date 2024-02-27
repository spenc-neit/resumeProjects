import { createBottomTabNavigator } from "@react-navigation/bottom-tabs";
import { FinalTabParams } from "./types/FinalTabParams";
import { MonsterStackNavigator } from "./MonsterStackNavigator";
import { ArmorBuilder } from "./ArmorBuilder";
import { MaterialsList } from "./MaterialsList";
import { Text } from "react-native";

export const FinalTabNavigator = () => {
    const FinalTab = createBottomTabNavigator<FinalTabParams>();
    return(
        <FinalTab.Navigator 
        screenOptions={({ route }) => ({
            tabBarIcon: ({ focused, color, size }) => {

                let iconName: any;

                switch(route.name){
                    case "Monster":
                        iconName = "MS";
                        break;
                    case "ArmorBuilder":
                        iconName = "AB";
                        break;
                    case "MaterialsList":
                        iconName = "ML";
                        break;
                    default:
                        iconName = "N/A";

                }

                return <Text style={{color: color, fontSize: size}}>{iconName}</Text>;

            },
            tabBarActiveTintColor: "#69F",
            tabBarInactiveTintColor: "gray",
            headerStyle:{
                backgroundColor:"#d8c7a9"
            },
            tabBarStyle:{
                backgroundColor: "#d8c7a9"
            }
        })}
        >
            <FinalTab.Screen name="Monster" component={MonsterStackNavigator} options={{title:"Monsters"}} />
            <FinalTab.Screen name="ArmorBuilder" component={ArmorBuilder} options={{title:"Loadout Builder"}} />
            <FinalTab.Screen name="MaterialsList" component={MaterialsList} options={{title:"Materials List"}} />
        </FinalTab.Navigator>
    )
}