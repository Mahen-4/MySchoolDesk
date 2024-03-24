
import { ModeToggle } from "./mode-toggle"
import { NavigationMenu, NavigationMenuContent, NavigationMenuItem, NavigationMenuLink, NavigationMenuList, NavigationMenuTrigger, navigationMenuTriggerStyle } from "@/components/ui/navigation-menu"
import { Link  } from 'react-router-dom';
import { Button } from "./ui/button";

function Navbar() {


    const role  :string = "ADMIN"

  return (
    <div className="flex">
        <NavigationMenu>
            <Link to="/" >
                <Button  variant="ghost">Home</Button>
            </Link>
            {
                role == "ADMIN" ? <Link to="/addStudent" >
                <Button  variant="ghost">addStudent</Button>
            </Link>
            :
            <></>
            } 
            
        </NavigationMenu>
        <ModeToggle />
    </div>
  )
}

export default Navbar
