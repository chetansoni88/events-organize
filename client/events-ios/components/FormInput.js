import React, { Component } from 'react';

import {
    StyleSheet,
    Text,
    TextInput,
    View
} from 'react-native';
import { WrappedInput } from './WrappedInput';
import { FontAwesome } from '@expo/vector-icons';
import Colors from '../constants/Colors';
import Sizes from '../constants/Sizes';

export class FormInput extends React.Component {

    render() {
        return (

            <View style={styles.incontainer}>
                <FontAwesome
                    name={this.props.fontIcon}
                    size={Sizes.tabIconSize}
                    color={Colors.tintColor}
                    style={{ marginTop: 8, width: Sizes.tabIconSize, height: Sizes.tabIconSize }}
                />
                <Text style={styles.label}>{this.props.labelText}</Text>
                <TextInput style={styles.input}
                    placeholder={this.props.placeholderValue}
                    onChangeText={this.props.onChangeText}
                    value={this.props.value}
                    secureTextEntry={this.props.secureTextEntry || false}
                />
            </View>
        );
    }
}
const styles = StyleSheet.create({
    incontainer: {
        flexDirection: 'row',
        height: 35,
        marginTop: 1
    },
    label: {
        fontSize: 14,
        color: Colors.labelColor,
        marginLeft: 5,
        marginTop: 8,
        height: 25,
        width: 65
    },
    input: {
        flex: 1,
        //alignItems: 'flex-start',
        color: Colors.inputColor,
        fontSize: 14,
        height: 35,
        marginLeft: 10,
    }
});
