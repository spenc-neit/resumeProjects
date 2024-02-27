export const titleCase = (input: string) => {

    let temp = input.split(' ')

    for(let i = 0; i < temp.length; i++){
        temp[i] = temp[i].charAt(0).toUpperCase() + temp[i].slice(1);
    }

    let output = ""
    
    temp.forEach(element => {
        output += `${element} `
    });

    output = output.trim();

	return output;
};
