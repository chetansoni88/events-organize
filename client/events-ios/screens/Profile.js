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
    EventSubscription,
    EventEmitter,
    Button
} from 'react-native';
import { FontAwesome } from '@expo/vector-icons';
import Colors from '../constants/Colors';
import Sizes from '../constants/Sizes';
import { FormInput } from '../components/FormInput';
import { CircleImageButton } from '../components/CircleImageButton';


export default class Profile extends React.Component {

    static route = {
        navigationBar: {
            //backgroundColor: Colors.tintColor,
            tintColor: Colors.tintColor,
            title: 'Profile',
            renderBackground: props => (
                <Image
                    source={{
                        uri: 'http://il9.picdn.net/shutterstock/videos/3951179/thumb/1.jpg',
                    }}
                    resizeMode={'cover'}
                />
            ),
            renderRight: (state) => {
                const { config: { eventEmitter } } = state;
                //const eventEmitter = new EventEmitter()
                return (
                    <View style={styles.logoutContainer}>
                        <TouchableOpacity onPress={() => { eventEmitter.emit('done') }} >
                            <FontAwesome name='sign-out'
                                size={Sizes.navigationRightIconSize}
                                color={Colors.tintColor} />
                        </TouchableOpacity>
                    </View>

                );
            },
            borderBottomWidth: Sizes.navigationBorderSize,
            borderBottomColor: Colors.tintColor,
        },
    };

    _subscriptionDone: EventSubscription;
    componentWillMount() {
        //this._subscriptionDone = this.props.route.getEmitter().addListener('done', this._handleDone);
    }

    componentWillUnmount() {
        //this._subscriptionDone.remove();
    }

    _handleDone = () => {
        console.log('Fired');

        if (this.props.navigator) {
            console.log(this.props.navigator.navigationBar);
        }
        else {
            console.log('false');
        }
        //this.props.navigator.pop();
    }

    render() {
        return (
            <ScrollView style={styles.container}>
                <FormInput labelText="Name" placeholderValue="John Doe" fontIcon="user" onChangeText={this._onNameChange} />
                <FormInput labelText="Email" placeholderValue="john@company.com" fontIcon="envelope" onChangeText={this._onNameChange} />
                <FormInput labelText="Password" placeholderValue="******" fontIcon="key" onChangeText={this._onNameChange} secureTextEntry={true}/>
                <FormInput labelText="Street" placeholderValue="Street name" fontIcon="compass" onChangeText={this._onStreetChange} />
                <FormInput labelText="City" placeholderValue="City" onChangeText={this._onCityChange} fontIcon="compass" />
                <FormInput labelText="State" placeholderValue="State" onChangeText={this._onStateChange} fontIcon="compass" />
                <FormInput labelText="Country" placeholderValue="Country" onChangeText={this._onCountryChange} fontIcon="compass" />
                <FormInput labelText="Zip" placeholderValue="Zip" onChangeText={this._onZipChange} fontIcon="compass" />
                <FormInput labelText="Phone" placeholderValue="Phone" onChangeText={this._onPhoneChange} fontIcon="compass" />

                <TouchableHighlight
                    style={styles.submit}
                    onPress={() => this._handleDone()}
                    underlayColor={Colors.tintColor}>
                    <Text style={[styles.submitText]}>Submit</Text>
                </TouchableHighlight>

            </ScrollView>
        );
    };
};

const styles =
    {
        container: {
            margin: 5
        },
        logoutContainer: {
            flexDirection: 'column',
            padding: 8,
            flex: 1,
            alignItems: 'center',
            marginRight: 3
        },
        submit: {
            marginRight: 40,
            marginLeft: 40,
            marginTop: 20,
            paddingTop: 20,
            paddingBottom: 20,
            backgroundColor: Colors.tintColor,
            borderRadius: 10,
            borderWidth: 1,
            borderColor: 'transparent'
        },
        submitText: {
            color: '#fff',
            textAlign: 'center',
        }
    }