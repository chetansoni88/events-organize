import React, { Component } from 'react';

import {
    StyleSheet,
    Text,
    TextInput,
    View
} from 'react-native';


export class WrappedInput extends React.Component {
    render() {
        return (
            <TextInput
                {...this.props}
                style={[this.props.style,styles.inputSearch]}
            />
        );
    }
}
const styles = StyleSheet.create({
    inputSearch: {
        backgroundColor: 'transparent',
        fontSize: 16,
        height: 25
    }
});
