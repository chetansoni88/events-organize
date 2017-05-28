import React from 'react';
import {
    Image,
    Linking,
    Platform,
    ScrollView,
    StyleSheet,
    Text,
    TextInput,
    TouchableOpacity,
    View,
    Button,
    Animated,
    Easing
} from 'react-native';
import Icon from 'react-native-vector-icons/FontAwesome';
import Container from '../components/Container';
import { WrappedInput } from '../components/WrappedInput';
import Colors from '../constants/Colors';
import { DatetimePicker } from '../components/DatetimePicker'
import { createRouter } from '@expo/ex-navigation';

var TIMES = 400;

export default class LoginScreen extends React.Component {
    static route = {
        navigationBar: {
            visible: false,
        },
    };

    press() {
        //execute any code here
        this.props.navigator.push('tabNavigation');
    }

    componentDidMount() {
        this._animate();
    }

    _animate() {
        this.state.angle.setValue(0);
        this._anim = Animated.timing(this.state.angle, {
            toValue: 360 * TIMES,
            duration: 6000 * TIMES,
            easing: Easing.linear
        }).start(this._animate);
    }

    state = {
        angle: new Animated.Value(0),
    }


    render() {
        return (
            <ScrollView style={styles.scroll}>
                <View style={styles.logoContainer}>
                    <Animated.Image
                        source={require('../assets/images/login.png')}
                        style={[
                            styles.image,
                            {
                                transform: [
                                    {
                                        rotate: this.state.angle.interpolate({
                                            inputRange: [0, 360],
                                            outputRange: ['0deg', '360deg']
                                        })
                                    },
                                ]
                            }]}>
                    </Animated.Image>

                    {/*<Image
                        source={require('../assets/images/login.png')}
                        style={styles.image}
                    />*/}
                </View>
                <View style={styles.loginContainer}>
                    <Container>
                        <WrappedInput
                            placeholder="Email address" />
                    </Container>
                    <Container>
                        <WrappedInput
                            placeholder="Password" secureTextEntry={true} />
                    </Container>
                    <View style={styles.signInButton}>
                        <Button
                            title="Sign In"
                            color={Colors.tintColor}
                            onPress={this.press.bind(this)} />
                    </View>
                    <View style={styles.helpContainer}>
                        <TouchableOpacity
                            onPress={this._handleHelpPress}
                            style={styles.helpLink}>
                            <Text style={styles.helpLinkText}>Join now</Text>
                        </TouchableOpacity>
                    </View>
                </View>
            </ScrollView>
        );
    };
};

const styles = StyleSheet.create({
    logoContainer: {
        alignItems: 'center',
        marginTop: 100
    }, image: {
        width: 100,
        height: 100,
    }, loginContainer: {
        margin: 20,
        marginTop: 80
    }, scroll: {
        backgroundColor: '#fff',
        margin: 10,
        flexDirection: 'column'
    }, signInButton: {
        marginTop: 20
    }, helpContainer: {
        marginTop: 5,
        alignItems: 'center'
    }, helpLinkText: {
        fontSize: 14,
        color: '#2e78b7',
    },
});