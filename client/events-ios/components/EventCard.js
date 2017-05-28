import React, { Component } from 'react';

import {
    StyleSheet,
    Text,
    View,
    TouchableHighlight
} from 'react-native';
import { WrappedInput } from './WrappedInput';
import { FontAwesome } from '@expo/vector-icons';
import Colors from '../constants/Colors';
import Sizes from '../constants/Sizes';

export class EventCard extends React.Component {

    render() {
        return (

            <View style={styles.incontainer}>
                <View style={styles.detailContainer}>
                    <Text style={styles.label}>{this.props.event.name}</Text>
                    <View style={{ height: 10 }} />
                    <Text style={styles.label}>{this.props.event.venue.street}</Text>
                    <Text style={styles.label}>{this.props.event.venue.city}, {this.props.event.venue.state}, {this.props.event.venue.zip}</Text>
                    <View style={{ height: 10 }} />
                    <Text style={styles.label}>{this._getDate(this.props.event.startdate)}</Text>
                    <View style={{ height: 10 }} />
                    <Text style={styles.label}>{this._getTypeDescription(this.props.event.type)}</Text>
                </View>
                <View style={[styles.iconContainer]}>
                    <TouchableHighlight onPress={this.props.onedit}
                        style={styles.icon} underlayColor='transparent'>
                        <FontAwesome
                            name={'pencil'}
                            size={40}
                            color={'white'}

                        />
                    </TouchableHighlight>

                    <TouchableHighlight onPress={this.props.onviewtimeline}
                        style={styles.icon} underlayColor='transparent'>
                        <FontAwesome
                            name={'calendar'}
                            size={40}
                            color={'white'}
                        />
                    </TouchableHighlight>
                </View>
            </View>
        );
    }

    _getTypeDescription = (type) => {

        if (type === 'Wedding') {
            return 'Wedding';
        }
        else if (type === 'Baby') {
            return 'Baby Shower';
        }
        else if (type === 'Private') {
            return 'Private Event';
        }
        else if (type === 'Corporate') {
            return 'Corporate Event';
        }
    };

    _getDate = (currentdate) => {

        var monthNames = ["January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        ];
        var datetime = monthNames[currentdate.getMonth()] + " "
            + currentdate.getDate() + ", " +
            + currentdate.getFullYear() + " @ "
            + currentdate.getHours() + ":"
            + currentdate.getMinutes();


        return datetime;
    }
}
const styles = StyleSheet.create({
    incontainer: {
        //flex:1,
        flexDirection: 'row',
        height: 120,
        //width: 300,
        backgroundColor: Colors.tintColor,
        margin: 5,
        borderRadius: 20,
        borderWidth: 2,
        borderColor: 'transparent'
    },
    detailContainer: {
        flexDirection: 'column',
        margin: 5,
        marginLeft: 10,
        flex: 0.8
    },
    iconContainer: {
        flex: 0.2,
        flexDirection: 'column',
        justifyContent: 'space-around'
    },
    icon: {
        margin: 2,
    },

    label: {
        color: 'white',
        fontFamily: 'Helvetica-BoldOblique',
        fontSize: 12,
    },

});
