import React from 'react';
import {
    Image,
    Linking,
    Platform,
    ScrollView,
    StyleSheet,
    Text,
    TouchableOpacity,
    TouchableHighlight,
    View,
    DatePickerIOS,
} from 'react-native';
import Colors from '../constants/Colors';
import Sizes from '../constants/Sizes';
import { FormInput } from '../components/FormInput';
import { FontAwesome } from '@expo/vector-icons';
import { CircleImageButton } from '../components/CircleImageButton';

export default class CreateEvent extends React.Component {

    state = {

        name: "",
        type: "",
        street: "", city: "", zip: "", country: "", state: ""
        ,
        arrangements: [],
        startdate: new Date()
    }

    _onDateChange = (date) => {
        console.log(date);
        this.setState({ startdate: date });
        this.props.newevent.startdate = date;
    };


    _onNameChange = (text) => {
        console.log(text);
        this.setState({ name: text });
        this.props.newevent.name = text;
    };

    _onStreetChange = (text) => {
        console.log(text);
        this.setState({ street: text });
        this.props.newevent.venue.street = text;
    };

    _onCityChange = (text) => {
        console.log(text);
        this.setState({ city: text });
        this.props.newevent.venue.city = text;
    };

    _onStateChange = (text) => {
        console.log(text);
        this.setState({ state: text });
        this.props.newevent.venue.state = text;
    };

    _onCountryChange = (text) => {
        console.log(text);
        this.setState({ country: text });
        this.props.newevent.venue.country = text;
    };

    _onZipChange = (text) => {
        console.log(text);
        this.setState({ zip: text });
        this.props.newevent.venue.zip = text;
    };

    _eventTypeSelected = (type) => {
        console.log(type);
        this.props.newevent.type = type;
    }

    render() {
        return (
            <ScrollView style={styles.container}>
                <FormInput labelText="Name" placeholderValue="John Doe" fontIcon="user" onChangeText={this._onNameChange} value={this.props.newevent.name} />
                <FormInput labelText="Street" placeholderValue="Street name" fontIcon="compass" onChangeText={this._onStreetChange} value={this.props.newevent.venue.street} />
                <FormInput labelText="City" placeholderValue="City" onChangeText={this._onCityChange} fontIcon="compass" value={this.props.newevent.venue.city} />
                <FormInput labelText="State" placeholderValue="State" onChangeText={this._onStateChange} fontIcon="compass" value={this.props.newevent.venue.state} />
                <FormInput labelText="Country" placeholderValue="Country" onChangeText={this._onCountryChange} fontIcon="compass" value={this.props.newevent.venue.country} />
                <FormInput labelText="Zip" placeholderValue="Zip" onChangeText={this._onZipChange} fontIcon="compass" value={this.props.newevent.venue.zip} />

                <ScrollView
                    horizontal={true}
                    pagingEnabled={true}
                    style={styles.horizontalScrollView}>
                    <CircleImageButton
                        src={require('../assets/images/wedding.png')}
                        onPress={() => { this._eventTypeSelected('Wedding'); }}
                        caption='Wedding' />
                    <CircleImageButton src={require('../assets/images/private.png')}
                        onPress={() => { this._eventTypeSelected('Private'); }}
                        caption='Private-Party' />
                    <CircleImageButton src={require('../assets/images/baby.png')}
                        onPress={() => { this._eventTypeSelected('Baby'); }}
                        caption='Baby-Shower' />
                    <CircleImageButton src={require('../assets/images/corporate.png')}
                        onPress={() => { this._eventTypeSelected('Corporate'); }}
                        caption='Corporate' />

                </ScrollView>

                <ScrollView
                    horizontal={true}
                    pagingEnabled={true}
                    style={styles.horizontalScrollView}>
                    <CircleImageButton
                        src={require('../assets/images/florist.png')}
                        onPress={() => { this._eventTypeSelected('Florist'); }}
                        caption='Florist' />
                    <CircleImageButton src={require('../assets/images/photographer.png')}
                        onPress={() => { this._eventTypeSelected('Photographer'); }}
                        caption='Photographer' />
                    <CircleImageButton src={require('../assets/images/caterer.png')}
                        onPress={() => { this._eventTypeSelected('Caterer'); }}
                        caption='Caterer' />
                    <CircleImageButton src={require('../assets/images/videographer.png')}
                        onPress={() => { this._eventTypeSelected('Videographer'); }}
                        caption='Videographer' />

                </ScrollView>
            </ScrollView>);
    }
};
const styles =
    {
        container: {
            flexDirection: 'column',
            marginRight: 2,
            marginLeft: 2,
            marginTop: 20,
            marginBottom: 10,
        },
        horizontalScrollView: {
            height: 75,
            marginTop: 25,
        }
    };