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
    Button,
    Modal,
    ListView,
} from 'react-native';
import Colors from '../constants/Colors';
import Sizes from '../constants/Sizes';
import { FontAwesome } from '@expo/vector-icons';
import CreateEvent from './CreateEvent'
import { EventCard } from '../components/EventCard'

export default class Events extends React.Component {

    constructor(props) {
        super(props);

        const ds = new ListView.DataSource({ rowHasChanged: (r1, r2) => r1 !== r2 });
        this.state = {
            modalVisible: false,
            events: [],
            currentEvent: null,
            dataSource: ds.cloneWithRows(this.state.events || []),
        }
        this._resetEvent();

    }

    static route = {
        navigationBar: {
            tintColor: Colors.tintColor,
            title: 'Events',
            renderBackground: props => (
                <Image
                    source={{
                        uri: 'http://il9.picdn.net/shutterstock/videos/3951179/thumb/1.jpg',
                    }}
                    resizeMode={'cover'}
                />
            ),
            renderRight: props => (
                <View style={styles.addContainer}>
                    <TouchableOpacity onPress={() => { }} >
                        <FontAwesome name='plus'
                            size={Sizes.navigationRightIconSize}
                            color={Colors.tintColor} />
                    </TouchableOpacity>

                </View>

            ),
            borderBottomWidth: Sizes.navigationBorderSize,
            borderBottomColor: Colors.tintColor,
        },
    };

    state = {

    }

    componentDidMount = () => {
        this._addEvent({
            id: '2868d451-9456-44fd-bc27-7d1393abe2df',
            name: "Mohit's Wedding",
            type: "Wedding",
            venue: {
                street: "688th 110 Ave NE", city: "Bellevue", zip: "98004", country: "USA", state: "WA"
            }
            ,
            arrangements: [],
            startdate: new Date()

        });
        this._addEvent({
            id: '062852d6-da80-427c-a9b5-ab32882cdb16',
            name: "Nina's Engagement",
            type: "Corporate",
            venue: {
                street: "132 River Dr", city: "Bellevue", zip: "86893", country: "USA", state: "WA"
            }
            ,
            arrangements: [],
            startdate: new Date()

        });
        this._addEvent({
            id: '288b8976-8357-4226-a317-1b8216a61f91',
            name: "Diya's Shower",
            type: "Baby",
            venue: {
                street: "130 iverLanding", city: "Charleston", zip: "29492", country: "USA", state: "SC"
            }
            ,
            arrangements: [],
            startdate: new Date()

        });
    }


    setModalVisible(visible) {
        this.setState({ modalVisible: visible });
    }

    _handleSubmit = () => {

        var f = this.state.events.find(e => e.id === this.state.currentEvent.id);

        if (f) {
            console.log('Found');
            this._editEvent(this.state.currentEvent);
        }
        else {
            console.log('Not Found');
            this._addEvent(this.state.currentEvent);
        }

        this._resetEvent();
        this.setModalVisible(false);
    };

    _editEvent = (event) => {
        var f = this.state.events.find(e => e.id === this.state.currentEvent.id);
        f = this.state.currentEvent;
        this.setState({
            dataSource: this.state.dataSource.cloneWithRows(this.state.events)
        });
        console.log(this.state.events);
    };

    _addEvent = (event) => {
        this.state.events.push(event);
        this.setState({
            dataSource: this.state.dataSource.cloneWithRows(this.state.events)
        })
    };

    _handleCancel = () => {
        this._resetEvent();
        this.setModalVisible(false);
    };

    _openModal = () => {
        this.setModalVisible(true);
    };

    _resetEvent = () => {
        this.state.currentEvent = {
            id: "",
            name: "",
            type: "",
            venue: {
                street: "", city: "", zip: "", country: "", state: ""
            }
            ,
            arrangements: [],
            startdate: new Date()

        }
    };

    _onEventEdit = (event) => {
        this.state.currentEvent = event;
        this._openModal();
    };

    _onViewTimeline = (event) => {
        console.log('timeline');
        console.log(event);
    };

    render() {
        return (
            <ScrollView>
                <Modal
                    animationType={"slide"}
                    transparent={false}
                    visible={this.state.modalVisible}
                    onRequestClose={() => { this._handleCancel(); }}
                >
                    <ScrollView style={{ marginTop: 22 }}>
                        <CreateEvent
                            newevent={this.state.currentEvent}
                        />

                        <View style={styles.actionGroup}>
                            <TouchableHighlight underlayColor='transparent'
                                style={styles.actionItem} onPress={() => {
                                    this._handleCancel();
                                }}>
                                <FontAwesome name='ban'
                                    size={Sizes.buttonSize}
                                    color={Colors.tintColor} />
                            </TouchableHighlight>
                            <TouchableHighlight underlayColor='transparent'
                                style={styles.actionItem} onPress={() => {
                                    this._handleSubmit();
                                }}>
                                <FontAwesome name='check-circle'
                                    size={Sizes.buttonSize}
                                    color={Colors.tintColor} />
                            </TouchableHighlight>
                        </View>
                    </ScrollView>
                </Modal>

                <TouchableHighlight
                    onPress={() => {
                        this._resetEvent();
                        this._openModal();
                    }}>
                    <Text>Add Event</Text>
                </TouchableHighlight>

                <ListView
                    dataSource={this.state.dataSource}
                    renderRow={(data) => <EventCard event={data}
                        onedit={() => { this._onEventEdit(data) }}
                        onviewtimeline={() => { this._onViewTimeline(data) }} />}
                    enableEmptySections={true}
                />

            </ScrollView>
        );
    };
};

const styles =
    {
        addContainer: {
            flexDirection: 'column',
            padding: 8,
            flex: 1,
            alignItems: 'center',
            marginRight: 3
        },
        actionGroup: {
            flexDirection: 'row',
            alignItems: 'center',
            justifyContent: 'center',
            height: 50,
            marginTop: 32,
        },
        actionItem: {
            padding: 12
        }
    }