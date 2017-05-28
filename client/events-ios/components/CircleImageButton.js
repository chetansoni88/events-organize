import React, { Component } from 'react';

import {
    StyleSheet,
    Text,
    TextInput,
    View,
    TouchableHighlight,
    Image,
} from 'react-native';
import { WrappedInput } from './WrappedInput';
import { FontAwesome } from '@expo/vector-icons';
import Colors from '../constants/Colors';
import Sizes from '../constants/Sizes';

export class CircleImageButton extends React.Component {

    state = {
        selected: false
    }

    render() {
        return (
            <View style={styles.view}>
                <TouchableHighlight style={styles.imageContainer} onPress={this._clicked()} underlayColor='transparent'>
                    <Image
                        source={this.props.src}
                        style={styles.image}
                    />
                </TouchableHighlight>
                <Text style={styles.caption}>{this.props.caption}</Text>
            </View>
        );
    }

    _clicked = () => {
        //this.setState({ selected: !this.state.selected });
        //this.props.onPress();
    }
}
const styles = StyleSheet.create({
    view: { flexDirection: 'column', height: 70 },
    imageContainer: {
        flex: 1,
        height: 50,
        width: 50,
        borderRadius: 25,
        borderColor: Colors.tintColor,
        margin: 15,
        marginTop: 1,
        marginBottom: 1,
        justifyContent: 'center',
        borderWidth: 1,

    },
    image: {
        height: 32,
        width: 32,
        marginLeft: 8.5,
    },
    caption: {
        color: Colors.tintColor,
        fontSize: 10,
        fontFamily: 'Helvetica-BoldOblique',
        textAlign: 'center',
        height: 20,
    }
});
